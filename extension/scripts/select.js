console.log('here we go')
window.addEventListener("DOMContentLoaded", () => {


    document.addEventListener("dblclick", (event) => {
        console.log(getSelectionWord())
    });

    document.addEventListener("mousedown", () => {
        // disappearButton();
        // whether user take a select action
        console.log('mousedown')
    });
});

function getSelectionWord() {
    let selection = window.getSelection();
    let text = "";
    let position;
    if (selection.rangeCount > 0) {
        text = selection.toString().trim();
        const lastRange = selection.getRangeAt(selection.rangeCount - 1);
        // If the user selects something in a shadow dom, the endContainer will be the HTML element and the position will be [0,0]. In this situation, we set the position undefined to avoid relocating the result panel.
        if (lastRange.endContainer !== document.documentElement) {
            let rect = selection.getRangeAt(selection.rangeCount - 1).getBoundingClientRect();
            position = [rect.left, rect.top];
        }
    }
    return { text, position };
}