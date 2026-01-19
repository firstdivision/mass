let quillInstances = {};

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

            // Create Save button with floppy disk icon
            const saveBtn = document.createElement('button');
            saveBtn.type = 'button';
            saveBtn.className = 'quill-toolbar-btn';
            saveBtn.title = 'Save';
            
            const saveIcon = document.createElement('i');
            saveIcon.className = 'bi bi-floppy-disk';
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
                cancelIcon.className = 'bi bi-x-lg';
                cancelBtn.appendChild(cancelIcon);
                
                cancelBtn.onclick = async () => {
                    await dotnetRef.invokeMethodAsync('HandleCancel');
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
