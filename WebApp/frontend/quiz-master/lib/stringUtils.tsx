export function truncateString(str: string, maxLength: number): string {
    if (maxLength >= str.length) {
        return str; // Return the original string if maxLength is greater than or equal to the length of the string
    } else {
        return str.slice(0, maxLength) + "..."; // Truncate the string and append "..."
    }
}