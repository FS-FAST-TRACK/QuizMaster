import { AuditTrail } from "@/lib/definitions";
import { create } from "zustand";

interface AuditStoreState {
    formattedFirstDate: string;
    formattedCurrentDate: string;
    currentDateAsString: string;
    actionTypeValue: string[] | undefined;
    searchedText: string;
    auditType: string | null;
    userAudit: AuditTrail[];
    filteredData: AuditTrail[];
    userType: string | null;
    actionType: string | null;
    setFormattedFirstDate: (date: string) => void;
    setFormattedCurrentDate: (date: string) => void;
    setSearchedText: (text: string) => void;
    setAuditType: (type: string | null) => void;
    setUserType: (userType: string | null) => void;
    setActionType: (actionType: string | null) => void;
    setActionTypeValue: (values: string[] | undefined) => void;
    setUserAudit: (data: AuditTrail[]) => void;
    setFilteredData: (data: AuditTrail[]) => void;
}

export const useAuditStore = create<AuditStoreState>((set) => ({
    formattedFirstDate: `${new Date().getFullYear()}-${String(
        new Date().getMonth() + 1
    ).padStart(2, "0")}-01`,
    formattedCurrentDate: `${new Date().getFullYear()}-${String(
        new Date().getMonth() + 1
    ).padStart(2, "0")}-${String(new Date().getDate()).padStart(2, "0")}`,
    currentDateAsString: new Date().toLocaleDateString("en-US", {
        month: "long",
        day: "numeric",
        year: "numeric",
    }),
    actionTypeValue: undefined,
    searchedText: "",
    auditType: "Account",
    userAudit: [],
    filteredData: [],
    userType: "All",
    actionType: "All",
    setFormattedFirstDate: (date) => set({ formattedFirstDate: date }),
    setFormattedCurrentDate: (date) => set({ formattedCurrentDate: date }),
    setSearchedText: (text) => set({ searchedText: text }),
    setAuditType: (type) => set({ auditType: type }),
    setUserType: (userType) => set({ userType: userType }),
    setActionType: (actionType) => set({ actionType: actionType }),
    setActionTypeValue: (values) => set({ actionTypeValue: values }),
    setUserAudit: (data) => set({ userAudit: data, filteredData: data }),
    setFilteredData: (data) => set({ filteredData: data }),
}));
