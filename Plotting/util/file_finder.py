import glob


def get_full_path(file_name):
    text_files = glob.glob("./**/" + file_name, recursive=True)

    if not text_files:
        raise Exception('Found no ' + file_name + ' file.')

    full_path = text_files[0]

    return full_path


def get_full_paths(file_name):
    text_files = glob.glob("./**/" + file_name, recursive=True)

    if not text_files:
        raise Exception('Found no ' + file_name + ' file.')

    return text_files
