export type RoomOptionTypes = (
    | "mode:normal"
    | "mode:elimination"
    | "showLeaderboardEachRound:true"
    | "showLeaderboardEachRound:false"
    | "displaytop10only:true"
    | "displaytop10only:false"
    | "allowreconnect:true"
    | "allowreconnect:false"
    | "allowjoinonquizstarted:true"
    | "allowjoinonquizstarted:false"
)[];

export interface CreateQuizRoom {
    roomName: string;
    questionSets: number[];
    roomOptions: RoomOptionTypes;
}

export interface QuizRoom extends CreateQuizRoom {
    id: number;
    roomName: "";
    qRoomDesc: string;
    qRoomPin: number;
    dateCreated: Date;
    dateUpdated?: Date;
    createdByUserId: number;
    updatedByUserId?: number;
}
