

export function range(start, end) {
    if (start > end) {
        return []
    }        
    return [...Array(1 + end - start).keys()].map(v => start + v)
}