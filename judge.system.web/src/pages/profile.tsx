import React from 'react';
import Topbar from '@/components/Topbar/Topbar';
import styles from '../styles/Profile.module.css';

const ProfilePage = () => {
  return (
    <div className="bg-dark-layer-2 min-h-screen">
      <Topbar/>
      <div className={styles.profileHeader}>
        
        <h1 className={styles.username}>YourUsername</h1>
        <p className={styles.rank}>Rank: 2,519,022</p>
        <button className={styles.editProfile}>Edit Profile</button>
      </div>
      <div className={styles.communityStats}>
        <h2>Community Stats</h2>
        <div className={styles.stats}>
          <div className={styles.statItem}>
            <p>Views</p>
            <p>0</p>
            <p>Last week 0</p>
          </div>
          <div className={styles.statItem}>
            <p>Solution</p>
            <p>0</p>
            <p>Last week 0</p>
          </div>
          <div className={styles.statItem}>
            <p>Discuss</p>
            <p>0</p>
            <p>Last week 0</p>
          </div>
          <div className={styles.statItem}>
            <p>Reputation</p>
            <p>0</p>
            <p>Last week 0</p>
          </div>
        </div>
      </div>
      <div className={styles.languages}>
        <h2>Languages</h2>
        <div className={styles.languageItem}>
          <span className={styles.languageName}>Python</span>
          <span className={styles.languageStat}>12 problems solved</span>
        </div>
        <div className={styles.languageItem}>
          <span className={styles.languageName}>C++</span>
          <span className={styles.languageStat}>3 problems solved</span>
        </div>
        <div className={styles.languageItem}>
          <span className={styles.languageName}>Python3</span>
          <span className={styles.languageStat}>3 problems solved</span>
        </div>
      </div>
      
    </div>
  );
};

export default ProfilePage;
