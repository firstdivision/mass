let quillInstances = {};
let initialContents = {};

export function initializeQuill(editorId, initialContent, placeholder, showButtons, showCancelButton, dotnetRef) {
    const container = document.getElementById(editorId);
    
    if (!container) {
        console.error(`Container with id ${editorId} not found`);
        return null;
    }

    const toolbarConfig = ['bold', 'italic', 'underline', 'clean'];

    const quill = new Quill(`#${editorId}`, {
        theme: 'snow',
        placeholder: placeholder,
        modules: {
            toolbar: toolbarConfig
        }
    });

    if (initialContent) {
        quill.root.innerHTML = initialContent;
    }

    // Store the initial content for change detection
    initialContents[editorId] = initialContent || '';

    // Add custom buttons to toolbar if needed
    if (showButtons) {
        const toolbar = document.querySelector(`#${editorId}`)?.previousElementSibling;
        if (toolbar && toolbar.classList.contains('ql-toolbar')) {
            // Ensure toolbar uses flexbox layout
            toolbar.style.display = 'flex';
            toolbar.style.alignItems = 'center';
            toolbar.style.flexWrap = 'wrap';
            toolbar.style.gap = '8px';

            // Create a spacer to push buttons to the right
            const spacer = document.createElement('div');
            spacer.style.marginLeft = 'auto';
            toolbar.appendChild(spacer);

            // Create a container for our buttons
            const buttonContainer = document.createElement('div');
            buttonContainer.className = 'quill-custom-buttons';
            buttonContainer.style.display = 'flex';
            buttonContainer.style.gap = '8px';
            buttonContainer.style.alignItems = 'center';

            // Create Save button with Bootstrap icon
            const saveBtn = document.createElement('button');
            saveBtn.type = 'button';
            saveBtn.className = 'quill-toolbar-btn';
            saveBtn.title = 'Save';
            
            const saveIcon = document.createElement('i');
            saveIcon.className = 'bi bi-floppy';
            saveBtn.appendChild(saveIcon);
            
            saveBtn.onclick = async () => {
                await dotnetRef.invokeMethodAsync('HandleSave');
            };

            buttonContainer.appendChild(saveBtn);

            // Create Cancel button if needed
            if (showCancelButton) {
                const cancelBtn = document.createElement('button');
                cancelBtn.type = 'button';
                cancelBtn.className = 'quill-toolbar-btn';
                cancelBtn.title = 'Cancel';
                
                const cancelIcon = document.createElement('i');
                cancelIcon.className = 'bi bi-x-circle';
                cancelBtn.appendChild(cancelIcon);
                
                cancelBtn.onclick = async () => {
                    const currentContent = quill.root.innerHTML;
                    const hasChanges = currentContent !== initialContents[editorId];
                    
                    if (hasChanges) {
                        showConfirmDialog('Cancel without saving changes?', async () => {
                            await dotnetRef.invokeMethodAsync('HandleCancel');
                        });
                    } else {
                        await dotnetRef.invokeMethodAsync('HandleCancel');
                    }
                };
                buttonContainer.appendChild(cancelBtn);
            }

            // Append buttons to the toolbar
            toolbar.appendChild(buttonContainer);
        }
    }

    quillInstances[editorId] = quill;
}

export function getQuillContent(editorId) {
    const quill = quillInstances[editorId];
    if (quill) {
        return quill.root.innerHTML;
    }
    return '';
}

export function setQuillContent(editorId, content) {
    const quill = quillInstances[editorId];
    if (quill) {
        quill.root.innerHTML = content;
    }
}

export function disposeQuill(editorId) {
    const quill = quillInstances[editorId];
    if (quill) {
        // Remove the toolbar that Quill creates
        const toolbar = document.querySelector(`#${editorId}`)?.previousElementSibling;
        if (toolbar && toolbar.classList.contains('ql-toolbar')) {
            toolbar.remove();
        }
    }
    delete quillInstances[editorId];
}

function showConfirmDialog(message, onConfirm) {
    // Create modal backdrop
    const backdrop = document.createElement('div');
    backdrop.className = 'modal-backdrop fade show';
    backdrop.style.display = 'block';

    // Create modal dialog
    const modal = document.createElement('div');
    modal.className = 'modal fade show';
    modal.style.display = 'block';
    modal.setAttribute('role', 'dialog');
    modal.setAttribute('aria-hidden', 'false');

    modal.innerHTML = `
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document" style="max-width: 90vw; margin: 1rem auto;">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Confirm</h5>
                </div>
                <div class="modal-body">
                    ${message}
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" id="cancelConfirmBtn">No, keep editing</button>
                    <button type="button" class="btn btn-danger" id="confirmBtn">Yes, cancel</button>
                </div>
            </div>
        </div>
    `;

    // Add to page
    document.body.appendChild(backdrop);
    document.body.appendChild(modal);

    // Handle button clicks
    const confirmBtn = modal.querySelector('#confirmBtn');
    const cancelConfirmBtn = modal.querySelector('#cancelConfirmBtn');

    function closeModal() {
        backdrop.remove();
        modal.remove();
        document.body.classList.remove('modal-open');
    }

    confirmBtn.onclick = async () => {
        closeModal();
        await onConfirm();
    };

    cancelConfirmBtn.onclick = () => {
        closeModal();
    };

    // Add modal-open class to body
    document.body.classList.add('modal-open');

    // Close on backdrop click
    backdrop.onclick = () => {
        closeModal();
    };
}

