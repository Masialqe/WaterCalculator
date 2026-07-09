window.showBootstrapModal = (id) => {
    const modalElement = document.getElementById(id);
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
}

window.hideBootstrapModal = (id) => {
    const modalElement = document.getElementById(id);
    const modal = bootstrap.Modal.getInstance(modalElement);
    if (modal) {
        modal.hide();
    }
}

window.appToast = {
    showSuccess: function (message, durationMs) {
        this.showToast(message, durationMs, {
            border: "border-emerald-500/30",
            background: "bg-emerald-500/10",
            text: "text-emerald-200",
            iconWrap: "border-emerald-400/30 bg-emerald-400/10",
            button: "border-emerald-500/30 text-emerald-200 hover:bg-emerald-500/10",
            icon: `
                <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" aria-hidden="true">
                    <path stroke-linecap="round" stroke-linejoin="round" d="m5 13 4 4L19 7"></path>
                </svg>
            `
        });
    },

    showFailure: function (message, durationMs) {
        this.showToast(message, durationMs, {
            border: "border-rose-500/30",
            background: "bg-rose-500/10",
            text: "text-rose-200",
            iconWrap: "border-rose-400/30 bg-rose-400/10",
            button: "border-rose-500/30 text-rose-200 hover:bg-rose-500/10",
            icon: `
                <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" aria-hidden="true">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M6 6l12 12M18 6 6 18"></path>
                </svg>
            `
        });
    },

    showToast: function (message, durationMs, styles) {
        const root = document.getElementById("app-toast-root");
        if (!root) return;

        const toast = document.createElement("div");
        toast.className =
            `pointer-events-auto mb-3 w-full max-w-3xl rounded-2xl border px-4 py-3 shadow-2xl shadow-black/30 backdrop-blur transition-opacity duration-300 ${styles.border} ${styles.background} ${styles.text}`;

        toast.innerHTML = `
            <div class="flex items-center justify-between gap-4">
                <div class="flex items-center gap-3">
                    <span class="inline-flex h-8 w-8 items-center justify-center rounded-full border ${styles.iconWrap}">
                        ${styles.icon}
                    </span>
                    <p class="text-sm font-medium"></p>
                </div>
                <button type="button"
                        class="rounded-lg border px-2 py-1 transition ${styles.button}"
                        aria-label="Zamknij powiadomienie">
                    ×
                </button>
            </div>
        `;

        const messageElement = toast.querySelector("p");
        const closeButton = toast.querySelector("button");

        if (messageElement) {
            messageElement.textContent = message;
        }

        const removeToast = () => {
            toast.classList.add("opacity-0");
            setTimeout(() => toast.remove(), 250);
        };

        closeButton.addEventListener("click", removeToast);

        root.appendChild(toast);

        setTimeout(removeToast, durationMs);
    }
};

window.appClipboard = {
    copyFromInputById: async function (inputId) {
        const input = document.getElementById(inputId);
        if (!input) {
            return false;
        }

        const value = input.value ?? "";
        if (!navigator.clipboard || !window.isSecureContext) {
            input.select();
            input.setSelectionRange(0, value.length);
            return document.execCommand("copy");
        }

        await navigator.clipboard.writeText(value);
        return true;
    },

    copyText: async function (text) {
        const value = text ?? "";
        if (!navigator.clipboard || !window.isSecureContext) {
            const textarea = document.createElement("textarea");
            textarea.value = value;
            textarea.style.position = "fixed";
            textarea.style.left = "-9999px";
            document.body.appendChild(textarea);
            textarea.focus();
            textarea.select();
            const result = document.execCommand("copy");
            document.body.removeChild(textarea);
            return result;
        }

        await navigator.clipboard.writeText(value);
        return true;
    }
};