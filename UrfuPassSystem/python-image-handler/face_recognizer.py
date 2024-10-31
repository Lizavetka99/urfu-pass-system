import os
import cv2
import numpy

class FaceRecognizer:
    def recognize(self, image : numpy.ndarray): # передается cv изображение
        # Обнаружение лиц
        x, y, w, h = self.get_face_rectangle(image)
        # cv2.rectangle(img, (x, y), (x+w, y+h), (255, 0, 0), 2)
        new_x = max(0, x - w * 0.5)
        new_y = max(0, y - h * 0.5)
        new_xw = min(image.shape[1], x + w * 1.5)
        new_yh = min(image.shape[0], y + h * 1.5)
        if new_xw >= image.shape[1]:
            new_xw = image.shape[1] - 1
        if new_yh >= image.shape[0]:
            new_yh = image.shape[0] - 1
        img = image[int(new_y):int(new_yh), int(new_x):int(new_xw)]
        return img

    def get_face_rectangle(self, image):
        face_cascade = cv2.CascadeClassifier(
            cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')
        gray_image = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        faces = face_cascade.detectMultiScale(gray_image, scaleFactor=1.1,
                                              minNeighbors=5,
                                              minSize=(30, 30))
        if len(faces) == 0:
            raise ValueError(2)
        if len(faces) > 1:
            raise ValueError(3)
        x, y, w, h = faces[0]
        return x, y, w, h
