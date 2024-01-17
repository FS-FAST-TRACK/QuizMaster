import { RefObject } from "react";

export interface QuestionCore {
    id: number;
    qAudio: string;
    qCategoryId: any;
    qDifficultyId: any;
    qTypeId: any;
    qImage: string;
    qStatement: string;
    qTime: any;
    details: QuestionDetail[];
}

export interface Question extends QuestionCore {
    qCategoryId: number;
    qDifficultyId: number;
    qTypeId: number;
    qTime: number;
    details: QuestionDetail[];
}

export interface QuestionEdit extends QuestionCore {
    qCategoryId: string;
    qDifficultyId: string;
    qTypeId: string;
    qTime: string;
}
export type QuestionDetail = {
    id: number;
    qDetailDesc: string;
    detailTypes: (
        | "answer"
        | "option"
        | "minimum"
        | "maximum"
        | "language"
        | "interval"
        | "range"
        | "textToAudio"
    )[];
};

export type DetailType = {
    id: number;
    detailDesc: string;
};
export type QuestionCategory = {
    id: number;
    qCategoryDesc: string;
    questionCounts: number;
    dateCreated: Date;
    dateUpdated: Date;
};
export type QuestionDifficulty = {
    id: number;
    qDifficultyDesc: string;
    dateCreated: Date;
    dateUpdated: Date;
    questionCounts: number;
};
export type QuestionType = {
    id: number;
    qTypeDesc: string;
};
export type QuestionCreateDto = {
    qAudio: string;
    qCategoryId: number;
    qDifficultyId: number;
    qTypeId: number;
    qImage: string;
    qStatement: string;
    qTime: number;
    questionDetailCreateDtos: QuestionDetailCreateDto[];
};
export type QuestionCreateValues = {
    qAudio: string;
    qCategoryId: string;
    qDifficultyId: string;
    qTypeId: string;
    qImage: string;
    qStatement: string;
    qTime: string;
    questionDetailCreateDtos: QuestionDetailCreateDto[];
    options: { value: string; isAnswer: boolean }[];
    trueOrFalseAnswer: boolean;
    minimum?: number;
    maximum?: number;
    interval?: number;
    sliderAnswer?: number;
    textToAudio?: string;
    language?: string;
    typeAnswer?: string;
};

export interface QuestionValues extends QuestionCreateValues {
    id: number;
    questionDetailDtos: QuestionDetail[];
}

export type QuestionDetailCreateDto = {
    qDetailDesc: string;
    detailTypes: string[];
};

export type ResourceParameter = {
    pageSize: string;
    searchQuery?: string;
    pageNumber: number;
};

export interface QuestionResourceParameter extends ResourceParameter {
    filterByCategories: number[];
    filterByDifficulties: number[];
    filterByTypes: number[];
    exludeQuestionsIds: number[] | undefined;
}

export type QuestionFilterProps = {
    filterByCategories: number[];
    filterByDifficulties: number[];
    filterByTypes: number[];
};
export interface CategoryResourceParameter extends ResourceParameter {
    isGetAll?: boolean;
}
export interface DifficultyResourceParameter extends ResourceParameter {
    isGetAll?: boolean;
}
export type PaginationMetadata = {
    totalCount: number;
    pageSize: number;
    currentPage: number;
    totalPages: number;
};
export type Set = {
    id: number;
    qSetName: string;
    qSetDesc: string;
    activeData: boolean;
    dateCreated: Date;
    dateUpdated: Date;
    createdByUserId: number;
    updatedByUserId: number;
};
export type SetDTO = {
    qSetDesc: string;
    qSetName: string;
    questions: number[];
};
export type QuestionSet = {
    questionId: number;
    setId: number;
    dateCreated: Date;
    dateUpdated: Date;
};
export type QuestionSetDTO = {
    qSetDesc: string;
    qSetName: string;
    questions: number[];
    dateCreated: Date;
    dateUpdated: Date;
};
export type PatchItem = {
    path: string;
    op: string;
    value: any;
};

export type UserAuditTrail = {
    UserAuditTrailId: number;
    UserId: number;
    Action: string;
    Timestamp: Date;
    Details: string;
    UserRole: string;
    Type: string;
};

export type UserAudit = {
    userAuditTrailId: number;
    userId: number;
    action: String;
    username: String;
    timestamp: Date;
    details: String;
    userRole: String;
    oldValues: String;
    newValues: String;
};

export type AuditTrail = {
    userAuditTrailId: number;
    userId: number;
    action: String;
    userName: String;
    timestamp: Date;
    details: String;
    userRole: String;
    oldValues: String;
    newValues: String;
};

export interface AuditTableProps {
    data: AuditTrail[];
    tableRef: RefObject<HTMLTableElement>;
    auditType: string | null;
}
