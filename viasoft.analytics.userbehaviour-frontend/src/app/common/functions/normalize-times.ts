export function getNormalizedInterval(startTime: Date, endTime: Date): { startTimeString: String, endTimeString: String } {
    const stringStartTime = (startTime.getHours() < 10 ? ("0" + startTime.getHours().toString()) : startTime.getHours().toString()) +
        ":" + (startTime.getMinutes() < 10 ? ("0" + startTime.getMinutes().toString()) : startTime.getMinutes().toString());
    const stringEndTime = (endTime.getHours() < 10 ? ("0" + endTime.getHours().toString()) : endTime.getHours().toString()) +
        ":" + (endTime.getMinutes() < 10 ? ("0" + endTime.getMinutes().toString()) : endTime.getMinutes().toString());
    return {
        startTimeString: stringStartTime,
        endTimeString: stringEndTime
    };
}

export function getNormalizedTime(time: number) {
    return time < 10 ? ("0" + time.toString()) : time.toString();
}

export function getNormalizedHourMinute(hour: string, minute: string) {
    return hour + ":" + minute;
}