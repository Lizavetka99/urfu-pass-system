import cv2

class ImageRotator:
    def rotate(self, image, face_square):
        start_x, start_y, width, height = face_square
        rotated_image = image
        if (width / height) >= 1.2:
            if start_x >= width / 3:
                rotated_image = cv2.rotate(image,
                                           cv2.ROTATE_90_COUNTERCLOCKWISE)
            else:
                rotated_image = cv2.rotate(image,
                                           cv2.ROTATE_90_CLOCKWISE)
        return rotated_image