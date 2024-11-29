import cv2, face_square_recogniser

class ImageRotator:
    def get_pixel_average(self, pixel):
        return sum(pixel) / len(pixel)

    def rotate(self, image):
        height, width = image.shape[0], image.shape[1]
        right_cof, left_cof, top_cof, bottom_cof = 0, 0, 0, 0
        for x in range(width):
            top_cof += self.get_pixel_average(image[3, x])
            bottom_cof += self.get_pixel_average(image[height - 4, x])
        for y in range(height):
            right_cof += self.get_pixel_average(image[y, width - 4])
            left_cof += self.get_pixel_average(image[y, 3])
        right_cof, left_cof, top_cof, bottom_cof = right_cof / height, left_cof / height, top_cof / width, bottom_cof / width
        min_cof = min(top_cof, left_cof, right_cof, bottom_cof)
        if (top_cof == min_cof):
            return cv2.rotate(image, cv2.ROTATE_180)
        elif (right_cof == min_cof):
            return cv2.rotate(image, cv2.ROTATE_90_CLOCKWISE)
        elif (left_cof == min_cof):
            return cv2.rotate(image, cv2.ROTATE_90_COUNTERCLOCKWISE)
        return image
