import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types'
import { range } from '../helpers';

const Pagination = props => {

    // << Prev 1 2 3 4 5 6 7 8 9 10 Next >>
    const { page,
        update,
        pageWindow,
        totalPages,
        location,
        className,
        id} = props    


    const pages = range(1, totalPages).map(x => <option key={x}>{x}</option>)

    return (
        <div className="pagination" id={id}>
            <select
                value={page}
                onChange={event => update(location, { page: Number(event.target.value) })}                
            >
                {pages}                
            </select>

            / {totalPages}
        </div>
        
    )

    //let prevPage = page - pageWindow
    //let nextPage = page + pageWindow

    //if (prevPage < 1)
    //    prevPage = 1

    //if (nextPage > totalPages) 
    //   nextPage = totalPages

    //return (
    //    <div className="pagination">
    //        <a onClick={() => update(location, { page: 1})}>First</a>
    //        <a onClick={() => update(location, { page: prevPage })}>Prev</a>
    //        {getPages(props)}
    //        <a onClick={() => update(location, { page: nextPage })}>Next</a>
    //        <a onClick={() => update(location, { page: totalPages })}>Last</a>
    //    </div>
    //)       
}

Pagination.propTypes = {
    location: PropTypes.string.isRequired,
    page: PropTypes.number.isRequired,
    totalPages: PropTypes.number.isRequired,
    pageWindow: PropTypes.number,
    update: PropTypes.func.isRequired
}

Pagination.defaultProps = {
    location: "",
    page: 1,
    totalPages: 1,
    pageWindow: 10,
    update: () => { }
}

const getFirstPageInWindow = ({ page, pageWindow }) =>
    (Math.floor((page - 1) / pageWindow) * pageWindow) + 1

const getPages = ({ location, page, totalPages, pageWindow, update }) => {
    const firstPage = getFirstPageInWindow({ page, pageWindow })
    const lastPage = firstPage + pageWindow - 1

    const start = firstPage < 1 ? 1 : firstPage
    const end = lastPage < totalPages ? lastPage : totalPages

    const pages = range(start, end).map(page => 
        <a key={page} onClick={() => {
            update(location, { page })
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