document.getElementById('orderForm').addEventListener('keydown', function(event) {
    if (event.key === 'Enter') {
        event.preventDefault(); 
    }
});
function updateFooterTotal() {
    let totalQuantity = 0;
    let totalPrice = 0;
    
    document.querySelectorAll('#itemsTableBody tr').forEach(function(row) {
        let qty = row.querySelector('input[name*="QUANTITY"]').value;
        let price = row.querySelector('input[name*="PRICE"]').value;
        let total = row.querySelector('input[name*="TOTAL"]');
        
        if (qty && price) {
            total.value = (qty * price).toFixed(2);
            
            totalQuantity += parseInt(qty);
            totalPrice += parseFloat(qty * price);
        }
    });
    
    document.getElementById('totalQuantity').textContent = totalQuantity;
    document.getElementById('totalPrice').textContent = totalPrice.toFixed(2);
}

updateFooterTotal();
function validateForm(event) {
    event.preventDefault();
    
    var form = $("#orderForm");
    var itemsTableBody = document.getElementById('itemsTableBody');
    var itemRows = itemsTableBody.querySelectorAll('tr');
    var validItems = false;
    
    itemRows.forEach(function(row) {
        var itemName = row.querySelector('input[name^="Form.Items"]').value;
        var quantity = row.querySelector('input[name$="QUANTITY"]').value;
        var price = row.querySelector('input[name$="PRICE"]').value;
        
        if (itemName && quantity && price) {
            validItems = true;
        }
    });

    if (!validItems) {
        $('#errorModal').modal('show');
        $('#errorMessage').text('Please add at least one item with valid information.');
    } else {
        if (form.valid()) {
            form.submit();
        }
    }
}

function updateTotal(inputElement) {
    updateFooterTotal()
    let row = inputElement.closest('tr');
    
    let quantity = parseFloat(row.querySelector('input[name$="QUANTITY"]').value) || 0;
    let price = parseFloat(row.querySelector('input[name$="PRICE"]').value) || 0;
    
    let total = quantity * price;

    let totalInput = row.querySelector('input[name$="TOTAL"]');
    totalInput.value = total.toFixed(2); 
}
function addItemRow() {
    let tableBody = document.getElementById('itemsTableBody');
    let rowCount = tableBody.rows.length + 1;
    let row = `
        <tr>
            <th scope="row">${rowCount}</th>
            <td>
                <button onclick="saveItemRow(this)" type="button" class="btn btn-primary btn-sm">
                    <i class="fa-solid fa-floppy-disk"></i>
                </button>
                <button onclick="deleteItemRow(this)" type="button" class="btn btn-danger btn-sm">
                    <i class="fa-solid fa-trash"></i>
                </button>
            </td>
            <td><input type="text" class="form-control" name="Form.Items[${rowCount - 1}].ITEM_NAME" /></td>
            <td><input type="number" class="form-control" onchange="updateTotal(this)" name="Form.Items[${rowCount - 1}].QUANTITY" /></td>
            <td><input type="number" class="form-control" onchange="updateTotal(this)" name="Form.Items[${rowCount - 1}].PRICE" /></td>
            <td><input type="number" class="form-control"  name="Form.Items[${rowCount}].TOTAL" readonly /></td>
        </tr>
    `;
    tableBody.insertAdjacentHTML('beforeend', row);
}

function saveItemRow(button) {
    // Get the row of the button clicked
    let row = button.closest('tr');
    
    // Disable inputs (make them readonly)
    let inputs = row.querySelectorAll('input');
    inputs.forEach(input => {
        input.readOnly = true;
    });

    // Change the "Save" button to "Edit"
    let saveButton = row.querySelector('.btn-primary');
    saveButton.innerHTML = '<i class="fa-solid fa-pen"></i>';
    saveButton.setAttribute('onclick', 'editItemRow(this)');
}

function editItemRow(button) {
    // Get the row of the button clicked
    let row = button.closest('tr');
    
    // Enable inputs for editing
    let inputs = row.querySelectorAll('input');
    inputs.forEach(input => {
        input.readOnly = false;
    });

    // Change the "Edit" button back to "Save"
    let saveButton = row.querySelector('.btn-primary');
    saveButton.innerHTML = '<i class="fa-solid fa-floppy-disk"></i>';
    saveButton.setAttribute('onclick', 'saveItemRow(this)');
}

function deleteItemRow(button) {
    let row = button.closest('tr');
    row.remove();
}

function selectCustomer(customer) {
    document.getElementById('customerDropdown').innerText = customer;
    document.getElementById('selectedCustomer').value = customer;
}
