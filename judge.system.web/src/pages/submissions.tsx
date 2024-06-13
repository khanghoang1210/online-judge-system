import React, { useEffect, useState } from 'react';
import styles from '../styles/Submissions.module.css';
import Topbar from '@/components/Topbar/Topbar';
import { toast } from "react-toastify";
import { jwtDecode } from 'jwt-decode';
import { useCookies } from 'react-cookie';
import { Router } from 'react-router-dom';
import "react-toastify/dist/ReactToastify.css";
import { useRouter } from 'next/router';
import SubmitionTable from '@/components/SubmissionTable';

export type Submission = {
  time: string;
  problemTitle: string;
  isAccepted: boolean;
  numCasesPassed: string;
  language: string;
};

interface SubmissionsProps {
  userName: string;
}

const Submissions: React.FC<SubmissionsProps> = ({ userName }) => {
  const [submissionList, setSubmissionList] = useState<Submission[]>([]);
  const router = useRouter();
  const [user, setUser] = useState<any>();
  const [cookie] = useCookies(["token"]);
  useEffect(() => {
    if (cookie.token) {
      setUser(jwtDecode(cookie.token));
    }
  }, [cookie.token]);

  // useEffect(() => {
  //   const fetchProblemDetails = async () => {
  //     if (!cookie.token) {
  //       toast.warn("Please login to access your submission", {
  //         position: "top-center",
  //         autoClose: 3000,
  //         theme: "dark"
  //       });
  //       return router.push('/auth');
       
  //     }
  //     console.log(cookie.token)
    
  //     try {
  //       const response = await fetch(
  //         `http://localhost:5107/api/submissions?userName=${user.nameid}`,
  //         {
  //           method: "GET",
  //           mode: "cors",
  //           headers: {
  //             'Authorization': `Bearer ${cookie.token}`,
  //             'Content-Type': 'application/json;charset=UTF-8',
  //             'Accept': 'application/json, text/plain',
  //         },
  //         }
  //       );
  //       const data = await response.json(); 
  //       setSubmissionList(data.data);
  //     } catch (error) {
  //       toast.error("Failed to fetch problem details");
  //     }
  //   };
    
  //   fetchProblemDetails();
  // }, []);

  console.log(submissionList);

  return (
    <div className="bg-dark-layer-2 min-h-screen" >
      <Topbar />
      <h1 className={styles.title}>All My Submissions</h1>
      <SubmitionTable/>
  
    </div>
  );
};
export default Submissions;