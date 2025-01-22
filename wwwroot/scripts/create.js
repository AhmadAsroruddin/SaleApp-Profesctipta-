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
    var validItems = true;
    
    let errorMessage = "Please add at least one item with valid information.";
    let unsavedChanges = false;

    itemRows.forEach(function(row, index) {
        var itemNameElement = row.querySelector(`input[name="Form.Items[${index}].ITEM_NAME"]`);
        var quantityElement = row.querySelector(`input[name="Form.Items[${index}].QUANTITY"]`);
        var priceElement = row.querySelector(`input[name="Form.Items[${index}].PRICE"]`);

        var itemName = itemNameElement ? itemNameElement.value.trim() : '';
        var quantity = quantityElement ? quantityElement.value.trim() : '';
        var price = priceElement ? priceElement.value.trim() : '';
        
        let isEditing = !itemNameElement.readOnly || !quantityElement.readOnly || !priceElement.readOnly;

        if (isEditing) {
            unsavedChanges = true;
        }

        if (itemName === "" || quantity === "" || price === "" || quantity <= 0 || price <= 0) {
            validItems = false;
            errorMessage = "Please ensure all fields are filled with valid values.";
        }

        if (itemName == "" && quantity == "" && price == "") {
            row.remove();  
        }
    });

    if (unsavedChanges) {
        $('#errorModal').modal('show');
        $('#errorMessage').text("Please save or discard changes before submitting the form.");
        return;
    }

    itemRows = itemsTableBody.querySelectorAll('tr'); 
    if (itemRows.length == 0) {
        validItems = false;
    }

    if (!validItems) {
        $('#errorModal').modal('show');
        $('#errorMessage').text(errorMessage);
    } else {
        form.submit();
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
    updateRowIndices()
}

function saveItemRow(button) {
    let row = button.closest('tr');
    
    let inputs = row.querySelectorAll('input');
    inputs.forEach(input => {
        input.readOnly = true;
    });

    let saveButton = row.querySelector('.btn-primary');
    saveButton.innerHTML = '<i class="fa-solid fa-pen"></i>';
    saveButton.setAttribute('onclick', 'editItemRow(this)');
}

function editItemRow(button) {
    let row = button.closest('tr');
    
    let inputs = row.querySelectorAll('input');
    inputs.forEach(input => {
        input.readOnly = false;
    });

    let saveButton = row.querySelector('.btn-primary');
    saveButton.innerHTML = '<i class="fa-solid fa-floppy-disk"></i>';
    saveButton.setAttribute('onclick', 'saveItemRow(this)');
}

function deleteItemRow(button) {
    let row = button.closest('tr');
    row.remove();
    updateRowIndices()
}

function selectCustomer(customer) {
    document.getElementById('customerDropdown').innerText = customer;
    document.getElementById('selectedCustomer').value = customer;
}


function updateRowIndices() {
    let tableBody = document.getElementById('itemsTableBody');
    let rows = tableBody.querySelectorAll('tr');
    
    rows.forEach((row, index) => {
        let indexCell = row.querySelector('th');  
        if (indexCell) {
            indexCell.textContent = index + 1; // Memperbarui nomor urut
        }

        let itemNameElement = row.querySelector('input[name*="ITEM_NAME"]');
        let quantityElement = row.querySelector('input[name*="QUANTITY"]');
        let priceElement = row.querySelector('input[name*="PRICE"]');
        let totalElement = row.querySelector('input[name*="TOTAL"]');

        // Memperbarui nama input sesuai dengan urutan baru
        if (itemNameElement) itemNameElement.setAttribute('name', `Form.Items[${index}].ITEM_NAME`);
        if (quantityElement) quantityElement.setAttribute('name', `Form.Items[${index}].QUANTITY`);
        if (priceElement) priceElement.setAttribute('name', `Form.Items[${index}].PRICE`);
        if (totalElement) totalElement.setAttribute('name', `Form.Items[${index}].TOTAL`);
    });
}
