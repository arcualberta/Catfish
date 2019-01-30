import React from "react";
import ReactDOM from "react-dom";
import { range } from './helpers';

const Pagination = ({
    location = "",
    currentPage = 1,
    totalPages = 1,
    pageWindow = 10,
    update = () => { }
}) => {

    // << Prev 1 2 3 4 5 6 7 8 9 10 Next >>

    let prevPage = currentPage - pageWindow
    let nextPage = currentPage + pageWindow

    if (prevPage < 1)
        prevPage = 1

    if nextPage


    return (
        <div>
            <a onClick={() => update(location, 1)}>First</a>
            <a onClick={() => update(location, prevPage)}>Prev</a>
            {getPages({ location, currentPage, totalPages, pageWindow, update })}
            <a onClick={() => update(location, nextPage)}>Next</a>
            <a onClick={() => update(location, totalPages)}>Last</a>
        </div>
    )
        
    
}

const getFirstPageInWindow = ({ currentPage, totalPages, pageWindow }) => 
    Math.round(currentPage / totalPages) * pageWindow + 1


const getPages = ({ location, currentPage, totalPages, pageWindow, update }) => {
    const firstPage = getFirstPageInWindow({ currentPage, totalPages, pageWindow })

    const pages = range(firstPage, firstPage + pageWindow - 1).map(page => 
        <a key={page} onClick={() => {
            update(location, page)
        }
        }> {page} </a>
    )

    return <span>{pages}</span>
}

export default Pagination

//const paginate = ({
//    currentPage = 1,
//    totalPages = 1, // end
//    delta = 2
//}) => {

//    let start = currentPage - delta,
//        end = currentPage + delta

//    if (totalPages > delta*2 + 1) {
       
//        if (start < 1) {
//            end -= start - 1
//            start = 1
//        }
        
//        if (end > totalPages) {
//            start = currentPage - 2*delta
//            end = totalPages
//        }
//    }
    
//    return range(start, end)
//}

//const getPages = ({
//    currentPage=1,
//    totalPages=1,
//    delta = 1,
//    listKey,
//    goToPage
//    }) => {

//    const pageRange = paginate({
//        currentPage,
//        totalPages,
//        delta
//    })
   
//    const paginas = pageRange.map((page) => 
//        <a key={page} onClick={() => {
//            goToPage({ listKey, page })
//        }
//        }> {page} </a>
//    )

//    return <span>{paginas}</span>

//}

//const Pagination = ({
//    currentPage = 1,
//    totalPages = 1,
//    delta = 1,
//    listKey,
//    goToPage}) => {

//    /*
//    << Prev 1 2 3 4 5 6 7 8 9 10 Next >>
//    */

//    const itemsPerPage = delta * 2 + 1
//    //const totalGroupPages = Math.floor(totalPages / stepDelta)
//    //const currentGroup = Math.floor(currentPage / stepDelta)

//    const centerPageDiff = delta + 1

//    const tempNextStep = Math.floor((currentPage / itemsPerPage) + 1) * itemsPerPage + centerPageDiff;
//    const tempPrevStep = Math.floor((currentPage / itemsPerPage) - 1) * itemsPerPage + centerPageDiff;

    
//    const nextStep = tempNextStep > totalPages ? totalPages : tempNextStep
//    const prevStep = tempPrevStep < 1 ? 1 : tempPrevStep

//    console.log("temp: " + tempPrevStep + " " + tempNextStep)
//    console.log("real " + prevStep + " " + nextStep)

//    //const tempNextStep = Math.floor(totalPages / currentPage) * stepDelta
//    //const tempPrevStep = currentPage - stepDelta
//    //const nextStep = tempNextStep > totalPages ? totalPages : tempNextStep
//    //const prevStep = tempPrevStep < 1 ? 1 : tempPrevStep

//    return (
//        <div>
//            <a onClick={() => goToPage({ listKey: listKey, page: 1 })}>First</a>
//            <a onClick={() => goToPage({ listKey: listKey, page: prevStep })}>Prev</a>
//            {getPages({
//                currentPage,
//                totalPages,
//                delta,
//                listKey,
//                goToPage
//            })}
//            <a onClick={() => goToPage({ listKey: listKey, page: nextStep })}>Next</a>
//            <a onClick={() => goToPage({ listKey: listKey, page: totalPages })}>Last</a>           
//        </div>
//        )
//}

//export default Pagination