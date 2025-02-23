export async function deleteQuestion(subthemeId: number, position: number, questionElement: HTMLElement): Promise<void> {
    try {
        const response = await fetch(`/api/Subthemes/${subthemeId}/questions/${position}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ position: position })
        });
        if (response.ok) {
            questionElement.remove();
        } 
    } catch (error) {
        console.error('Error deleting question:', error);
    }
}
