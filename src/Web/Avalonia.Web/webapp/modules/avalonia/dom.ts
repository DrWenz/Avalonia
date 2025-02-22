export class AvaloniaDOM {
    public static addClass(element: HTMLElement, className: string): void {
        element.classList.add(className);
    }

    static createAvaloniaHost(host: HTMLElement) {
        // Root element
        host.classList.add("avalonia-container");
        host.tabIndex = 0;
        host.oncontextmenu = function () { return false; };

        // Rendering target canvas
        const canvas = document.createElement("canvas");
        canvas.classList.add("avalonia-canvas");
        canvas.style.backgroundColor = "#ccc";
        canvas.style.width = "100%";
        canvas.style.height = "100%";
        canvas.style.position = "absolute";

        // Native controls host
        const nativeHost = document.createElement("div");
        nativeHost.classList.add("avalonia-native-host");
        nativeHost.style.left = "0px";
        nativeHost.style.top = "0px";
        nativeHost.style.width = "100%";
        nativeHost.style.height = "100%";
        nativeHost.style.position = "absolute";

        // IME
        const inputElement = document.createElement("input");
        inputElement.classList.add("avalonia-input-element");
        inputElement.autocapitalize = "none";
        inputElement.type = "text";
        inputElement.spellcheck = false;
        inputElement.style.padding = "0";
        inputElement.style.margin = "0";
        inputElement.style.position = "absolute";
        inputElement.style.overflow = "hidden";
        inputElement.style.borderStyle = "hidden";
        inputElement.style.outline = "none";
        inputElement.style.background = "transparent";
        inputElement.style.color = "transparent";
        inputElement.style.display = "none";
        inputElement.style.height = "20px";
        inputElement.onpaste = function () { return false; };
        inputElement.oncopy = function () { return false; };
        inputElement.oncut = function () { return false; };

        host.prepend(inputElement);
        host.prepend(nativeHost);
        host.prepend(canvas);

        return {
            host,
            canvas,
            nativeHost,
            inputElement
        };
    }
}
