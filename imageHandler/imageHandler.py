import cv2
import numpy as np

def check_for_min_quality(image):
    """Возвращает True, если изображение подходит по качеству"""
    height, width = image.shape[:2]
    return height >= 480 and width >= 360  # Данные размеры соответствуют >= 300 dpi для фото 3х4.


def remove_transparency(image):
    """Возвращает изображение без прозрачности"""
    pass


def identify_face(image):
    """Возвращает True, если одно лицо на изображении"""
    face_count = 0
    prototxt_path = 'weights/deploy.prototxt.txt'
    model_path = 'weights\\res10_300x300_ssd_iter_140000.caffemodel'

    model = cv2.dnn.readNetFromCaffe(prototxt_path, model_path)
    blob = cv2.dnn.blobFromImage(image, 1.0, (300, 300), (104.0, 177.0, 123.0))

    model.setInput(blob)
    output = np.squeeze(model.forward())

    for i in range(0, output.shape[0]):
        confidence = output[i, 2]
        if confidence > 0.8:
            face_count += 1
    return face_count == 1


def turn_face(image):
    """Возвращает изображение лица в нужной ориентации"""
    prototxt_path = 'weights/deploy.prototxt.txt'
    model_path = 'weights\\res10_300x300_ssd_iter_140000.caffemodel'

    model = cv2.dnn.readNetFromCaffe(prototxt_path, model_path)
    h, w = image.shape[:2]
    blob = cv2.dnn.blobFromImage(image, 1.0, (300, 300), (104.0, 177.0, 123.0))

    model.setInput(blob)
    output = np.squeeze(model.forward())

    rotated_image = image
    for i in range(0, output.shape[0]):
        confidence = output[i, 2]
        print(confidence)
        if confidence > 0.8:
            box = output[i, 3:7] * np.array([w, h, w, h])
            start_x, start_y, end_x, end_y = box.astype(int)
            width = end_x - start_x
            height = end_y - start_y
            print(width, height, width / height)
            if (width / height) >= 1.2:
                if start_x >= width / 3:
                    rotated_image = cv2.rotate(rotated_image,
                                               cv2.ROTATE_90_COUNTERCLOCKWISE)
                else:
                    rotated_image = cv2.rotate(rotated_image,
                                               cv2.ROTATE_90_CLOCKWISE)
        return rotated_image


def check_for_blur(image):
    """Возвращает False, если изображение слишком размыто"""
    minimumQuality = 100

    #  Еще нужно тестить на множестве фотографий, желательно на квадратах лиц.
    #  Минимальное значение "чёткости" пока условно.

    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    return int(cv2.Laplacian(gray, cv2.CV_64F).var()) >= minimumQuality


def cut_quality(image):
    """Возвращает изображение, урезанное по качеству,
    если изображение слишком большое"""
    pass


def rename_image(image_name):
    """Возвращает название изображения, соответствующее критериям"""
    pass


def save_image(image):
    """Сохраняет изображение в папку"""
    pass
