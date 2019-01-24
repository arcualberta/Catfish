import React from "react";
import ReactDOM from "react-dom";
import { range } from './helpers';

const paginate = ({
    currentPage = 1,
    totalPages = 1, // end
    delta = 2
}) => {

    let start = currentPage - delta,
        end = currentPage + delta

    if (totalPages > delta*2 + 1) {
       
        if (start < 1) {
            end -= start - 1
            start = 1
        }
        
        if (end > totalPages) {
            start = currentPage - 2*delta
            end = totalPages
        }
    }
    console.log (start + " " + end)
    return range(start, end)
}

const getPages = ({
    currentPage=1,
    totalPages=1,
    delta = 1,
    goToPage
    }) => {

    const pageRange = paginate({
        currentPage,
        totalPages,
        delta
    })
    console.log(goToPage)
    const paginas = pageRange.map((page) => <a key={page} onClick={() => { goToPage(page) }}> {page} </a>)

    return <div>{paginas}</div>

}

const Pagination = ({
    currentPage = 1,
    totalPages = 1,
    delta = 1,
    goToPage}) => {

    /*
    << Prev 1 2 3 4 5 6 7 8 9 10 Next >>
    */

    return (
        <div>
            <a >First</a>
            <a >Prev</a>
            {getPages({
                currentPage,
                totalPages,
                delta,
                goToPage
            })}
            <a >Next</a>
            <a >Last</a>           
        </div>
        )
}

export default Pagination