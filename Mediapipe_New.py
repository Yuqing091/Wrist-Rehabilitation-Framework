import mediapipe as mp
import cv2
import numpy as np
import uuid
import os
from matplotlib import pyplot as plt
import zmq
#from memory_profiler import profile


# Setup zmq TCP Sockets
context = zmq.Context()
socket = context.socket(zmq.PUSH)
socket.connect('tcp://localhost:5555')

mp_drawing = mp.solutions.drawing_utils
mp_hands = mp.solutions.hands

joint_list = [[7,6,5], [11,10,9], [15,14,13], [19,18,17]]


def draw_finger_angles(image, results, joint_list):
    store_angle = []
    # Loop through hands
    for hand in results.multi_hand_landmarks:
        #Loop through joint sets 
        for joint in joint_list:
            a = np.array([hand.landmark[joint[0]].x, hand.landmark[joint[0]].y]) # First coord
            b = np.array([hand.landmark[joint[1]].x, hand.landmark[joint[1]].y]) # Second coord
            c = np.array([hand.landmark[joint[2]].x, hand.landmark[joint[2]].y]) # Third coord
            
            radians = np.arctan2(c[1] - b[1], c[0]-b[0]) - np.arctan2(a[1]-b[1], a[0]-b[0])
            angle = np.abs(radians*180.0/np.pi)
            
            if angle > 180.0:
                angle = 360-angle

            store_angle.append(angle)
            
              
            cv2.putText(image, str(round(angle, 2)), tuple(np.multiply(b, [640, 480]).astype(int)),
                       cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 255, 255), 2, cv2.LINE_AA)
    #print("%s\r" %store_angle, end = "", flush = True)
    socket.send_json(store_angle)
    
    # Check if the socket buffer is full
    poller = zmq.Poller()
    poller.register(socket, zmq.POLLIN)

    is_buffer_full = not poller.poll(10)  # 1 second timeout
    if is_buffer_full:
        socket.disconnect("tcp://localhost:5555")
        socket.connect("tcp://localhost:5555")
    else:
        print("Socket buffer is not full")

    store_angle.clear()     
    return image


def get_label(index, hand, results):
    output = None
    for idx, classification in enumerate(results.multi_handedness):
        if classification.classification[0].index == index:
            
            # Process results
            label = classification.classification[0].label
            score = classification.classification[0].score
            text = '{} {}'.format(label, round(score, 2))
            
            # Extract Coordinates
            coords = tuple(np.multiply(
                np.array((hand.landmark[mp_hands.HandLandmark.WRIST].x, hand.landmark[mp_hands.HandLandmark.WRIST].y)),
            [640,480]).astype(int))
            
            output = text, coords
            
    return output

cap = cv2.VideoCapture(0)





with mp_hands.Hands(min_detection_confidence=0.8, min_tracking_confidence=0.5) as hands: 
    try:
        while cap.isOpened():
            ret, frame = cap.read()
            
            # BGR 2 RGB
            image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            
            # Flip on horizontal
            image = cv2.flip(image, 1)
            
            # Set flag
            image.flags.writeable = False
            
            # Detections
            results = hands.process(image)

            # Set flag to true
            image.flags.writeable = True
            
            # RGB 2 BGR
            image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
            # Rendering results
            if results.multi_hand_landmarks:
                for num, hand in enumerate(results.multi_hand_landmarks):
                    
                    # Render hand landmarks
                    mp_drawing.draw_landmarks(image, hand, mp_hands.HAND_CONNECTIONS,
                                            mp_drawing.DrawingSpec(color=(121, 22, 76), thickness=2, circle_radius=4),
                                            mp_drawing.DrawingSpec(color=(250, 44, 250), thickness=2, circle_radius=2),
                                            )
                    # Get hand label
                    label = get_label(num, hand, results)
                    
                    # Display label
                    if label:
                        text, coord = label
                        cv2.putText(image, text, coord, cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 255, 255), 2, cv2.LINE_AA)
                        
                # Draw angles
                image = draw_finger_angles(image, results, joint_list)
                
            # Display image
            cv2.imshow('Hand Tracking', image)

            '''cv2.waitKey(1)
            
            # Close window
            if cv2.getWindowProperty('Hand Tracking',cv2.WND_PROP_VISIBLE) < 1:        
                break'''
            
            # Release resources
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break
                
    except Exception as e:
        print("Error occurred: ",e)

    finally:
        cap.release()
        cv2.destroyAllWindows()

