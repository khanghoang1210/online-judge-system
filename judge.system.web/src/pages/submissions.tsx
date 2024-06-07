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

const Submissions: React.FC = ({  }) => {
  return (
    <div className="bg-dark-layer-2 min-h-screen" >
      <Topbar />
      <h1 className={styles.title}>All My Submissions</h1>
      <SubmitionTable/>
  
    </div>
  );
};
export default Submissions;