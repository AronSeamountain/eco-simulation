def extract(key, data):
    return [i[key] for i in data]


def filter_where(data, key, value_equals):
    output = []

    for row in data:
        if row[key] == value_equals:
            output.append(row)

    return output


def extract_unique(key, data):
    output = []

    for item in extract(key, data):
        if not output.__contains__(item):
            output.append(item)

    return output
