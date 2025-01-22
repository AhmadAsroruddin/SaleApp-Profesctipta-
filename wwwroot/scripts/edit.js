document.getElementById('orderForm').addEventListener('keydown', function(event) {
    if (event.key === 'Enter') {
        event.preventDefault(); 
    }
});
function updateTotals() {
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

updateTotals();

function validateForm(event) {
    event.preventDefault();
    
    var form = $("#orderForm");
    var itemsTableBody = document.getElementById('itemsTableBody');
    var itemRows = itemsTableBody.querySelectorAll('tr');
    var validItems = true;
    
    let errorMessage = "Please add at least one item with valid information."
    itemRows.forEach(function(row, index) {
        var itemNameElement = row.querySelector(`input[name="Form.Items[${index}].ITEM_NAME"]`);
        var quantityElement = row.querySelector(`input[name="Form.Items[${index}].QUANTITY"]`);
        var priceElement = row.querySelector(`input[name="Form.Items[${index}].PRICE"]`);

        var itemName = itemNameElement ? itemNameElement.value.trim() : '';
        var quantity = quantityElement ? quantityElement.value.trim() : '';
        var price = priceElement ? priceElement.value.trim() : '';
        console.log(itemName +" " + index)
        if (itemName === "" || quantity === "" || price === "" || quantity <= 0 || price <= 0) {
            validItems = false;
            errorMessage = "Please ensure all fields are filled with valid values.";
        }
        if (itemName == "" && quantity == "" && price == "" ) {
            row.remove();  
        }
    });

    itemRows = itemsTableBody.querySelectorAll('tr'); 
    if (itemRows.length == 0) {
        console.log(itemRows.length);
        validItems = false;
    }

    if (!validItems) {
        $('#errorModal').modal('show');
        $('#errorMessage').text(errorMessage);
    } else {
        form.submit();
    }
}

function addItemRow() {
    let tableBody = document.getElementById('itemsTableBody');
    let rowCount = tableBody.querySelectorAll('tr').length; // Hitung ulang item yang ada
    let row = `
        <tr>
            <th scope="row">${rowCount + 1}</th>
            <td>
                <button onclick="editItemRow(this)" type="button" class="btn btn-primary btn-sm">
                    <i class="fa-solid fa-pen"></i>
                </button>
                <button onclick="deleteRow(this)" type="button" class="btn btn-danger btn-sm">
                    <i class="fa-solid fa-trash"></i>
                </button>
            </td>
            <td><input readonly type="text" class="form-control" name="Form.Items[${rowCount}].ITEM_NAME" /></td>
            <td><input readonly type="number" class="form-control" name="Form.Items[${rowCount}].QUANTITY" onchange="updateTotal(this)" /></td>
            <td><input readonly type="number" class="form-control" name="Form.Items[${rowCount}].PRICE" onchange="updateTotal(this)" /></td>
            <td><input readonly type="number" class="form-control" name="Form.Items[${rowCount}].TOTAL" readonly /></td>
        </tr>
    `;
    tableBody.insertAdjacentHTML('beforeend', row);
    updateRowIndices();  
}


function updateTotal(inputElement) {
    updateTotals();
    let row = inputElement.closest('tr');
    
    let quantity = parseFloat(row.querySelector('input[name$="QUANTITY"]').value) || 0;
    let price = parseFloat(row.querySelector('input[name$="PRICE"]').value) || 0;
    
    let total = quantity * price;

    let totalInput = row.querySelector('input[name$="TOTAL"]');
    totalInput.value = total.toFixed(2); 
}

function saveItemRow(button) {
    let row = button.closest('tr');
    let inputs = row.querySelectorAll('input');
    inputs.forEach(input => {
        input.readOnly = true;
        input.classList.add('saved-item');
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

function deleteRow(button) {
    let row = button.closest('tr');
    row.remove();  // Hapus baris
    updateRowIndices();  // Update nomor urut setelah penghapusan
}


function deleteItemRow(button) {
    let row = button.closest('tr');
    let itemId = row.querySelector('input[type="hidden"]').value; 
    $.ajax({
        url: '/Order/DeleteItem',
        type: 'POST',
        data: { itemId: itemId },
        success: function (response) {
            row.remove();
            updateRowIndices()
        },
        error: function (error) {
            console.log(error)
            alert("Error: " + error);
        }
    });
}

function updateRowIndices() {
    let tableBody = document.getElementById('itemsTableBody');
    let rows = tableBody.querySelectorAll('tr');
    
    // Untuk setiap baris, update nomor urut dan atribut name input
    rows.forEach((row, index) => {
        // Update nomor urut
        let indexCell = row.querySelector('th');  // Mengambil elemen yang berisi nomor urut
        if (indexCell) {
            indexCell.textContent = index + 1;  // Mengatur nomor urut menjadi index + 1
        }

        // Memperbarui nama input agar sesuai dengan urutan baris yang benar
        let itemNameElement = row.querySelector('input[name*="ITEM_NAME"]');
        let quantityElement = row.querySelector('input[name*="QUANTITY"]');
        let priceElement = row.querySelector('input[name*="PRICE"]');
        let totalElement = row.querySelector('input[name*="TOTAL"]');

        if (itemNameElement) itemNameElement.setAttribute('name', `Form.Items[${index}].ITEM_NAME`);
        if (quantityElement) quantityElement.setAttribute('name', `Form.Items[${index}].QUANTITY`);
        if (priceElement) priceElement.setAttribute('name', `Form.Items[${index}].PRICE`);
        if (totalElement) totalElement.setAttribute('name', `Form.Items[${index}].TOTAL`);
    });
}
