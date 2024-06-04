export function tableSort(event, colIndex) {
    //Сортировка
    const table = document.querySelector('table');
    //let colIndex = -1;

    const sortTable = function (index, type, isSorted) {
        const tbody = table.querySelector('tbody');

        const compare = function (row1, row2) {
            var rowData1 = row1.cells[index].innerHTML;
            var rowData2 = row2.cells[index].innerHTML;

            switch (type) {
                case 'int':
                    return rowData1 - rowData2;
                case 'double': {
                    rowData1 = rowData1.replace(',', '.');
                    rowData2 = rowData2.replace(',', '.');
                    parseFloat(rowData1);
                    parseFloat(rowData2);
                    return rowData1 - rowData2;
                }
                case 'date': {
                    rowData1 = rowData1.split('.').reverse();
                    rowData2 = rowData2.split('.').reverse();
                    return new Date(rowData1).getTime() - new Date(rowData2).getTime();
                }
                case 'string':
                    if (rowData1 < rowData2) return -1;
                    else if (rowData1 > rowData2) return 1;
                    return 0;
            }
        }
        let rows = [].slice.call(tbody.rows);

        rows.sort(compare);

        if (isSorted) {
            rows.reverse();
        }

        table.removeChild(tbody);

        for (let i = 0; i < rows.length; i++) {
            tbody.appendChild(rows[i]);
        }
        table.appendChild(tbody);
    }

    if (table) {
        const el = event.target;
        if (el.nodeName != 'TH')
            return;

        const index = el.cellIndex;
        const type = el.getAttribute('data-type')

        sortTable(index, type, colIndex == index);

        if (colIndex == index) {
            colIndex = -1;
        }
        else
            colIndex = index;
    };
    return colIndex;
}
    