class QualityChecker:
    def check_for_min_quality(self, image):
        """Возвращает True, если изображение подходит по качеству"""
        height, width = image.shape[:2]
        return height >= 1200 and width >= 900  # Данные размеры соответствуют >= 300 dpi для фото 3х4.
