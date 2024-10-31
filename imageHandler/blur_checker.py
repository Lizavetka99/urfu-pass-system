import cv2


class BlurChecker:
    def check_for_blur(self, image):
        """Возвращает False, если изображение слишком размыто"""
        minimumQuality = 0

        #  Еще нужно тестить на множестве фотографий, желательно на квадратах лиц.
        #  Минимальное значение "чёткости" пока условно.

        gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        return int(cv2.Laplacian(gray, cv2.CV_64F).var()) >= minimumQuality