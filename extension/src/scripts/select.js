import { initialize, dispose } from "../components/popover";

const root = document.createElement("div");

const ButtonPositionSetting = "TopRight";

window.addEventListener("DOMContentLoaded", () => {

    document.addEventListener("dblclick", (event) => {
        const selectedWord = getSelectionWord();

        if (!selectedWord) return

        showButton(event, selectedWord.text)
    });

    document.addEventListener("mousedown", (e) => {

        if (e.target?.id === 'translate-btn' || e.target.closest('#rvocabular-drawer')) {
            return
        }

        console.log(e)


        root.remove()
        dispose()
    });
});


function getSelectionWord() {
    let selection = window.getSelection();
    let text = "";
    let position;
    if (selection.rangeCount > 0) {
        text = selection.toString().trim();
        const lastRange = selection.getRangeAt(selection.rangeCount - 1);

        if (lastRange.endContainer !== document.documentElement) {
            let rect = selection.getRangeAt(selection.rangeCount - 1).getBoundingClientRect();
            position = [rect.left, rect.top];
        }
    }
    return { text, position };
}


function showButton(event, value) {
    root.id = "rvocabular-root"

    const OffsetXValue = 10, OffsetYValue = 20;
    let XBias, YBias;
    switch (ButtonPositionSetting) {
        default:
        case "TopRight":
            XBias = OffsetXValue;
            YBias = -OffsetYValue;
            break;
        case "TopLeft":
            XBias = -OffsetXValue;
            YBias = -OffsetYValue;
            break;
        case "BottomRight":
            XBias = OffsetXValue;
            YBias = OffsetYValue;
            break;
        case "BottomLeft":
            XBias = -OffsetXValue;
            YBias = OffsetYValue;
            break;
    }

    let XPosition = event.x + XBias;
    let YPosition = event.y + YBias;

    root.style.top = `${YPosition}px`;
    root.style.left = `${XPosition}px`;

    document.documentElement.appendChild(root);
    initialize(value)
}