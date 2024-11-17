import cv2


class BlurChecker:
    def check_for_blur(self, image, face_square):
        """Возвращает False, если изображение слишком размыто"""
        minimumQuality = 200
        x, y, w, h = face_square
        gray = cv2.cvtColor(image[y:y + h, x:x + w], cv2.COLOR_BGR2GRAY)
        return int(cv2.Laplacian(gray, cv2.CV_64F).var()) >= minimumQuality