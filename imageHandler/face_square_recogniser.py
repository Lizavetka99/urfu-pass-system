import cv2


class FaceSquareRecognizer:
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
        return (x, y, w, h)