
    // function beforeDelete( )
    //{
    //    return(confirm('Are you sure you want to delete the selected item?'));
    //}


var el = document.getElementById('btnDelete');
el.onclick = showfunc;


function showfunc() {
    alert('Are you sure you want to delete the selected item?');
}