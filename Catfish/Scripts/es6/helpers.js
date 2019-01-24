

export function objectify(toObjectify) {
    return JSON.parse(JSON.stringify(toObjectify))
}

// XXX improve
export function range(start, end) {
    
    let result = []
    for (let i = start; i <= end; ++i) {
        result.push(i)
    }

    return result
}