import React from "react";
import moment from "moment";

interface TimeDateProps {
    startTime: Date;
    endTime: Date;
}

export default function TimeDateDuration({
    startTime,
    endTime,
}: TimeDateProps) {
    const formatTime = (date: Date): string => {
        return moment(date).format("h:mm A");
    };

    const formatDate = (date: Date): string => {
        return moment(date).format("MMM DD, YYYY");
    };

    const calculateDuration = (start: Date, end: Date): string => {
        const durationMs = moment(end).diff(moment(start));
        const duration = moment.duration(durationMs);
        const hours = duration.hours();
        const minutes = duration.minutes();
        return `${hours}hr ${minutes}min`;
    };

    return (
        <div>
            <p className="text-gray-800 font-semibold">
                {formatDate(startTime)}
            </p>
            <div className="flex-row">
                <p className="text-gray-600 text-xs mt-2">
                    {formatTime(startTime)} - {formatTime(endTime)} •{" "}
                    {calculateDuration(startTime, endTime)}
                </p>
            </div>
        </div>
    );
}
