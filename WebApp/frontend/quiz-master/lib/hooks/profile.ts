import { QUIZMASTER_ACCOUNT_GET, QUIZMASTER_ACCOUNT_PATCH, QUIZMASTER_AUTH_GET_COOKIE_INFO } from "@/api/api-routes";
import { IAccount, UserAccount } from "@/store/ProfileStore";
import { Dispatch, SetStateAction } from "react";
import { notification } from "../notifications";

export async function getUserInfo(){
  try{
    const response = await fetch(`${QUIZMASTER_AUTH_GET_COOKIE_INFO}`,{
      credentials: "include"
    });

    const data = await response.json();
    const isInvalidCredentials = response.status === 401;

    const userData = data.info.userData as IAccount;
    return {userData:new UserAccount().parse(userData), roles:data.info.roles as Array<string>};
  }catch(e){console.log("Error ",e)}
  return {userData:new UserAccount(), roles:[""]};;
}

export async function getAccountInfo(id: Number){
  try{
    const response = await fetch(`${QUIZMASTER_ACCOUNT_GET}/${id}`,{
      credentials: "include"
    });

    const data = await response.json();

    const userData = data as IAccount;
    return userData;
  }catch(e){console.log("Error ",e)}
  return new UserAccount();
}

export async function updateEmail(id: Number, newEmail: string, ErrorCallback: (message: string) => void, RefreshCallback?: () => void) {
  if(!validateEmail(newEmail)){
    ErrorCallback("Invalid Email Address");
    RefreshCallback && RefreshCallback();
    return;
  }
  const response = await fetch(`${QUIZMASTER_ACCOUNT_PATCH}${id}`, {
    method:"PATCH", 
    credentials: "include", 
    headers: {"content-type":"application/json"},
    body:JSON.stringify([{path: "email", op: "replace", value: newEmail}])});

  const data = await response.json()
  if(response.status !== 200){
    ErrorCallback(data.message);
    RefreshCallback && RefreshCallback();
  }else {notification(data);}
}

export async function saveUserDetails(account: IAccount, setEditToggle: Dispatch<SetStateAction<boolean>>, ErrorCallback: (message: string) => void){
  // apply patch
  let payload = new Array<object>;
  for(let prop in account){
    if(prop === "id") continue;
    payload.push({
      path: prop.toLowerCase(),
      op: "replace",
      value: account[prop as keyof IAccount]
    })
  }
  const response = await fetch(`${QUIZMASTER_ACCOUNT_PATCH}${account.id}`, {
    method:"PATCH", 
    credentials: "include", 
    headers: {"content-type":"application/json"},
    body:JSON.stringify(payload)});
  
  const data = await response.json()
  if(response.status !== 200){
    ErrorCallback(data.message);
  }else {setEditToggle(false);notification(data)}}

function validateEmail(email: string) {
    const res = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
    return res.test(String(email).toLowerCase());
}