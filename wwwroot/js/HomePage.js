console.log($)

$('.product-save-btn').on('change', function () {
    console.log('Heart icon clicked');
    var productId = this.id.split('-')[0];
    var isChecked = this.checked;
    fetch('/Home/ToggleSaveProduct', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ productId: productId, save: isChecked }),
    })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
            if (error.response.status === 401) {
                window.location.href = '/Welcome/Index';
            } else {
                console.error('Error saving product:', error);
            }
        });
});
