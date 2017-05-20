var editingIndexRow = -1;
var editingTextboxId = "";
var editingClientId = "";
var editingDataField = "";
var firstEditableDataField = "ProductName";

function Grid1_onLoad(sender, e) {
    Grid1_addRow();
}
function saveCell(itemId, columnField, newValue) {
    var row = Grid1.GetRowFromClientId(itemId);
    // Check if value was changed
    var oldValue = row.GetMember(columnField).Value;
    if (oldValue != newValue) {
        // Get column index for SetValue
        var col = 0;
        for (var i = 0; i < Grid1.Table.Columns.length; i++) {
            if (Grid1.Table.Columns[i].DataField == columnField) {
                col = i;
                break;
            }
        }
        row.SetValue(col, newValue, true);
    }
    return true;
}
function Grid1_addRow() {
    var col = 0;
    if (Grid1.get_table().getRowCount() > 0) {
        // cancel adding row if there is already a blank one
        var row = Grid1.get_table().getRow(Grid1.get_table().getRowCount() - 1);
        for (var i = 0; i < Grid1.Table.Columns.length; i++) {
            if (row.getMemberAt(i).get_text() != "") {
                Grid1.get_table().addRow();
                break;
            }
        }
    }
    else {
        Grid1.get_table().addRow();
    }
    setTimeout("setTextboxFocus();", 100);
}
function setTextboxFocus(datafield, index) {
    if (typeof datafield == "undefined") datafield = firstEditableDataField;
    if (typeof index == "undefined") index = Grid1.get_recordCount() - Grid1.get_recordOffset() - 1;
    var elementId = "textbox_" + index + "_" + datafield;
    try {
        document.getElementById(elementId).focus();
    }
    catch (err) {
    }
}

function EditField_onKeyPress(e) {
    if (!e) e = window.event;
    key = e.keyCode ? e.keyCode : e.which;
    if (key == 13) //enter
    {
        saveCell(editingClientId, editingDataField, document.getElementById(editingTextboxId).value);
        Grid1_addRow();
    }
    if (key == 38) //up
    {
        var index = editingIndexRow - 1;
        if (index > -1) {
            editingIndexRow = index;
            saveCell(editingClientId, editingDataField, document.getElementById(editingTextboxId).value);
            setTextboxFocus(editingDataField, index);
        }
    }
    if (key == 40) //down
    {
        var index = editingIndexRow + 1;
        if (index < Grid1.get_pageSize()) {
            editingIndexRow = index;
            saveCell(editingClientId, editingDataField, document.getElementById(editingTextboxId).value);
            setTextboxFocus(editingDataField, index);
        }
    }
}
