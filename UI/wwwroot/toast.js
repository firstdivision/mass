function showToast(message) {
    const toastBody = document.getElementById('toastBody');
    const toastElement = document.getElementById('toast');
    
    if (!toastBody || !toastElement) return;
    
    toastBody.textContent = message;
    
    const toast = new bootstrap.Toast(toastElement, {
        autohide: true,
        delay: 3000
    });
    
    toast.show();
}
