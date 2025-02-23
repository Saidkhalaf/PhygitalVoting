/*const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootsrap.Tooltip(tooltipTriggerEl))*/

let copybtns = Array.from(document.querySelectorAll(".btn-copy"))
copybtns.forEach(copybtn => copybtn.addEventListener("click", () => copy(copybtn)))

function copy(copybtn){

    navigator.clipboard.writeText(copybtn.value);
}