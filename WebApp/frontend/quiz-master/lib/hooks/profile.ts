import { QUIZMASTER_ACCOUNT_DELETE, QUIZMASTER_ACCOUNT_GET, QUIZMASTER_ACCOUNT_PASSWORD_RESET_POST, QUIZMASTER_ACCOUNT_PATCH, QUIZMASTER_AUTH_GET_COOKIE_INFO } from "@/api/api-routes";
import { IAccount, UserAccount } from "@/store/ProfileStore";
import { Dispatch, SetStateAction } from "react";
import { notification } from "../notifications";
import { signOut } from "next-auth/react";

export async function getUserInfo(){
  try{
    const response = await fetch(`${QUIZMASTER_AUTH_GET_COOKIE_INFO}`,{
      credentials: "include"
    });

    const data = await response.json();
    const isInvalidCredentials = response.status === 401;

    const userData = data.info.userData as IAccount;
    return {userData:new UserAccount().parse(userData), roles:data.info.roles as Array<string>};
  }catch(e){console.error("Error ",e)}
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
  }catch(e){console.error("Error ",e)}
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
  
  if(!account.firstName || !account.lastName || !account.userName){
    ErrorCallback("Make sure that the fields are not empty");
    return
  }
  if(account.firstName && account.firstName.length < 3){
    ErrorCallback("Firstname must have at least 3 characters");
    return
  }
  if(account.lastName && account.lastName.length < 3){
    ErrorCallback("Lastname must have at least 3 characters");
    return
  }
  if(account.userName && account.userName.length < 5){
    ErrorCallback("Username must have at least 5 characters");
    return
  }
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

export async function UpdatePassword(id: Number,password: Array<string>, setEditToggle: Dispatch<SetStateAction<boolean>>, ErrorCallback: (message: string) => void){
  const response = await fetch(QUIZMASTER_ACCOUNT_PASSWORD_RESET_POST(id), {
    method:"POST", 
    credentials: "include", 
    headers: {"content-type":"application/json"},
    body:JSON.stringify({currentPassword: password[0], newPassword: password[1]})});
    const data = await response.json()
  if(response.status !== 200){
    ErrorCallback(data.message);
    return false;
  }else {setEditToggle(false);notification(data); return true;}
}


export async function DeleteAccount(id: Number){
  await fetch(`${QUIZMASTER_ACCOUNT_DELETE}${id}`, {
    method:"DELETE", 
    credentials: "include"});
  signOut();
  return {message:"Delete account success", success:true};
}