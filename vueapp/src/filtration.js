export function tableFiltr() {
    //Поиск
    var selectElem = document.getElementById('IdCombobox');
    var index = 1;
    if (selectElem) {
        selectElem.addEventListener('change', function () {
            index = selectElem.selectedIndex;
        })
    }

    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("Input");
    filter = input.value.toUpperCase();
    table = document.getElementById("table_id");
    tr = table.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[index];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            }
            else {
                tr[i].style.display = "none";
            }
        }
    }
}
    