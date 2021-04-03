def extract(key, data):
    return [i[key] for i in data]


def extract_unique(key, data):
    output = []

    for item in extract(key, data):
        if not output.__contains__(item):
            output.append(item)

    return output
