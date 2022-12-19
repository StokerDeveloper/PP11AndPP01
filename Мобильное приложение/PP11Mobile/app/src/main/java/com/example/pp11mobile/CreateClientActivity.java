package com.example.pp11mobile;

import androidx.appcompat.app.AppCompatActivity;

import android.app.DatePickerDialog;
import android.os.Bundle;
import android.text.format.DateUtils;
import android.view.View;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TextView;

import java.util.Calendar;

public class CreateClientActivity extends AppCompatActivity {
    EditText editTextFamilia;
    EditText editTextIma;
    EditText editTextOtchestvo;
    TextView currDate;
    Calendar dateAndTime = Calendar.getInstance();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_client);

        editTextFamilia = findViewById(R.id.editTextTextFamilia);
        editTextIma = findViewById(R.id.editTextTextIma);
        editTextOtchestvo = findViewById(R.id.editTextTextOtchestvo);
        currDate = findViewById(R.id.clientDate);
    }

    public void back(View view) {
        this.finish();
    }

    public void setDate(View view) {
        new DatePickerDialog(CreateClientActivity.this, d,
                dateAndTime.get(Calendar.YEAR),
                dateAndTime.get(Calendar.MONTH),
                dateAndTime.get(Calendar.DAY_OF_MONTH))
                .show();
    }

    private void setInitialDateTime() {
        currDate.setText(DateUtils.formatDateTime(this,
                dateAndTime.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
    }

    DatePickerDialog.OnDateSetListener d = new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateAndTime.set(Calendar.YEAR, year);
            dateAndTime.set(Calendar.MONTH, monthOfYear);
            dateAndTime.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };

    public void create(View view) {
        String familia = editTextFamilia.getText().toString();
        String ima = editTextIma.getText().toString();
        String otchestvo = editTextOtchestvo.getText().toString();

        MainActivity.clientAdapter.add(familia + " " + ima + " " + otchestvo);
        MainActivity.clientAdapter.notifyDataSetChanged();

        this.finish();
    }
}