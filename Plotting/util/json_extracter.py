def extract(key, data):
    return [i[key] for i in data]


def extract_unique(key, data):
    # Gets all the values.
    output = []

    for item in extract(key, data):
        if not output.__contains__(item):
            output.append(item)

    return output


def extract_unique_get_object(key, data):
    # Gets all the dictionaries.
    output = []
    added = []

    for item in data:
        if not added.__contains__(item[key]):
            added.append(item[key])
            output.append(item)

    return output
