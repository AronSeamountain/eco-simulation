import shutil

from util.file_finder import get_full_path

fps_filename = 'fps.csv'
overview_filename = 'overview.csv'

fps_baseline_filename = fps_filename.replace('.', '_baseline.')
overview_baseline_filename = overview_filename.replace('.', '_baseline.')


def snapshot():
    fps_full_path = get_full_path(fps_filename)
    fps_full_path_baseline = fps_full_path.replace(fps_filename, fps_baseline_filename)

    overview_full_path = get_full_path(overview_filename)
    overview_full_path_baseline = overview_full_path.replace(overview_filename, overview_baseline_filename)

    print(fps_full_path)

    shutil.copy2(fps_full_path, fps_full_path_baseline)
    print('Created ' + fps_full_path_baseline)

    shutil.copy2(overview_full_path, overview_full_path_baseline)
    print('Created ' + overview_full_path_baseline)


if __name__ == "__main__":
    snapshot()
