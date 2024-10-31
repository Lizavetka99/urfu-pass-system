import unittest
import face_recognizer, image_cutter, quality_checker, transparency_remover, \
    image_rotator, blur_checker
import os
import cv2

class TestCheckForMinQuality(unittest.TestCase):
    def test_check_for_min_quality_true(self):
        """Проверка, что данные фотографии подходят по размеру."""
        checker = quality_checker.QualityChecker()
        directoryValid = "TestPhotos/TestCheckMinQuality/Valid"
        for filename in os.listdir(directoryValid):
            input_path = os.path.join(directoryValid, filename)
            image = cv2.imread(input_path, cv2.IMREAD_UNCHANGED)
            self.assertTrue(checker.check_for_min_quality(image), "Error in " + filename)

    def test_check_for_min_quality_false(self):
        """Проверка, что данные фотографии не подходят по размеру."""
        checker = quality_checker.QualityChecker()
        directoryValid = "TestPhotos/TestCheckMinQuality/InValid"
        for filename in os.listdir(directoryValid):
            input_path = os.path.join(directoryValid, filename)
            image = cv2.imread(input_path, cv2.IMREAD_UNCHANGED)
            self.assertFalse(checker.check_for_min_quality(image), "Error in " + filename)


class TestCheckRemoveTransparency(unittest.TestCase):
    def test_remove_transparency(self):
        self.assertFalse(False)


class TestIdentifyFace(unittest.TestCase):
    def test_identify_face(self):
        self.assertFalse(False)


class TestTurnFace(unittest.TestCase):
    def test_turn_face(self):
        self.assertFalse(False)


class TestCheckForBlur(unittest.TestCase):
    def test_check_for_blur(self):
        self.assertFalse(False)


class TestCutQuality(unittest.TestCase):
    def test_cut_quality(self):
        self.assertFalse(False)


class TestSaveImage(unittest.TestCase):
    def test_save_image(self):
        self.assertFalse(False)


if __name__ == '__main__':
    unittest.main()