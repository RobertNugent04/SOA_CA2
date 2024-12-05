import React, { useState } from "react";
import "./userCard.css";

type EditUserProps = {
  fullName: string;
  bio: string | null;
  onSave: (updatedData: { fullName: string; bio: string; profilePicture: File | null }) => void;
  onClose: () => void;
};

export const EditUser: React.FC<EditUserProps> = ({ fullName, bio, onSave, onClose }) => {
  const [updatedFullName, setUpdatedFullName] = useState(fullName);
  const [updatedBio, setUpdatedBio] = useState(bio || "");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setSelectedFile(event.target.files[0]);
    }
  };

  const handleSave = () => {
    onSave({
      fullName: updatedFullName,
      bio: updatedBio,
      profilePicture: selectedFile,
    });
  };

  return (
    <div className="edit-user-overlay">
      <div className="edit-user-box">
        <h2>Edit Profile</h2>
        <label>
          Full Name:
          <input
            type="text"
            value={updatedFullName}
            onChange={(e) => setUpdatedFullName(e.target.value)}
            className="edit-user-input"
          />
        </label>
        <label>
          Bio:
          <textarea
            value={updatedBio}
            onChange={(e) => setUpdatedBio(e.target.value)}
            className="edit-user-textarea"
          />
        </label>
        <label>
          Upload Profile Picture:
          <input type="file" onChange={handleFileChange} className="edit-user-file-input" />
        </label>
        <div className="edit-user-actions">
          <button onClick={handleSave} className="edit-user-save-button">
            Save
          </button>
          <button onClick={onClose} className="edit-user-cancel-button">
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};
