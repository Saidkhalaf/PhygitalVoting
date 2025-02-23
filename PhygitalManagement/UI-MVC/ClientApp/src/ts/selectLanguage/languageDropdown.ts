languageSelector();
function languageSelector(): void {
    document.querySelectorAll('.dropdown-item').forEach(item => {
        item.addEventListener('click', () => {
            const languageForm = document.getElementById('languageForm') as HTMLFormElement | null;
            if (languageForm) {
                languageForm.submit();
            }
        });
    });
}
