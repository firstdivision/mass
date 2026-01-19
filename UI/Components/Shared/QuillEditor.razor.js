let quillInstances = {};

export function initializeQuill(editorId, initialContent, placeholder) {
    const container = document.getElementById(editorId);
    
    if (!container) {
        console.error(`Container with id ${editorId} not found`);
        return null;
    }

    const quill = new Quill(`#${editorId}`, {
        theme: 'snow',
        placeholder: placeholder,
        modules: {
            toolbar: [
                ['bold', 'italic', 'underline', 'clean']
            ]
        }
    });

    if (initialContent) {
        quill.root.innerHTML = initialContent;
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
