import moment from "moment";

export const formatDateTimeRange = (startTime: Date, endTime: Date): string => {
    // Format start time
    const formattedStartTime = moment(convertToBrowserTimezone(startTime)).format("MMMM DD, YYYY HH:mm A");

    // Format end time
    const formattedEndTime = moment(convertToBrowserTimezone(endTime)).format("HH:mm A");

    // Calculate duration in hours and minutes
    const durationHours = moment
        .duration(moment(endTime).diff(moment(startTime)))
        .hours();
    const durationMinutes = moment
        .duration(moment(endTime).diff(moment(startTime)))
        .minutes();

    // Construct the formatted string
    const formattedString = `${formattedStartTime} - ${formattedEndTime} • ${durationHours? durationHours+"h":""} ${durationMinutes}m`;

    return formattedString;
};

export function parseDateStringToDate(dateString: string) {
    // Split the date string into its components
    const dateParts = dateString.split(" ");
    const date = dateParts[0].split("/");
    const time = dateParts[1].split(":");
    const day = parseInt(date[0], 10);
    const month = parseInt(date[1], 10) - 1; // Months are 0-indexed
    const year = parseInt(date[2], 10);
    let hour = parseInt(time[0], 10);
    const minute = parseInt(time[1], 10);
    const second = parseInt(time[2], 10);

    // Adjust hour for AM/PM
    if (dateParts[2].toLowerCase() === "pm" && hour !== 12) {
        hour += 12;
    } else if (dateParts[2].toLowerCase() === "am" && hour === 12) {
        hour = 0;
    }

    // Create and return the Date object
    return new Date(year, month, day, hour, minute, second);
}

export function convertToBrowserTimezone(utcDateTime: Date): Date {
    const dateTime = new Date(utcDateTime);
    // Get the browser's timezone offset in milliseconds
    const browserTimezoneOffset = new Date().getTimezoneOffset() * 60 * 1000;

    // Calculate the local time by adding the browser's timezone offset to the UTC time
    const localTime = new Date(dateTime.getTime() - browserTimezoneOffset);

    return localTime;
}