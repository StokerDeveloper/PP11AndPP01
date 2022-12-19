package com.example.pp11mobile;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Random;

public class MainActivity extends AppCompatActivity {
    EditText orderNumber;
    public static Spinner client;
    Spinner service;
    ListView servicesList;
    EditText hoursNumber;

    public static ArrayAdapter<String> clientAdapter;
    ArrayAdapter<String> serviceAdapter;
    ArrayAdapter<String> servicesListAdapter;

    public static ArrayList<String> clients = new ArrayList<String>();
    ArrayList<String> services = new ArrayList<String>();
    ArrayList<String> selectedServices = new ArrayList<String>();

    ArrayList<Integer> servicesPrices = new ArrayList<Integer>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Collections.addAll(clients, "Викторов Егор Иванович", "Иванов Иван Иванович", "Демигин Роман Викторович");
        Collections.addAll(services, "Прокат коньков", "Прокат детских коньков", "Катание на катке");
        Collections.addAll(servicesPrices, 100, 200, 300);

        orderNumber = findViewById(R.id.editTextTextOrderNumber);
        client = findViewById(R.id.client);
        service = findViewById(R.id.service);
        servicesList = findViewById(R.id.servicesList);
        hoursNumber = findViewById(R.id.editTextNumber);

        orderNumber.setText("" + ((new Random()).nextInt(9000) + 1000));
        hoursNumber.setText("" + 1);

        clientAdapter = new ArrayAdapter(this, android.R.layout.simple_spinner_item, clients);
        clientAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        client.setAdapter(clientAdapter);

        serviceAdapter = new ArrayAdapter(this, android.R.layout.simple_spinner_item, services);
        serviceAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        service.setAdapter(serviceAdapter);

        servicesListAdapter = new ArrayAdapter(this, android.R.layout.simple_list_item_multiple_choice, selectedServices);
        servicesList.setAdapter(servicesListAdapter);

        servicesList.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View v, int position, long id) {
                String service = servicesListAdapter.getItem(position);
                servicesListAdapter.remove(service);
            }
        });
    }

    public void createClient(View view) {
        Intent intent = new Intent(this, CreateClientActivity.class);
        startActivity(intent);
    }

    public void addService(View view) {
        String selectedService = service.getSelectedItem().toString();
        servicesListAdapter.add(selectedService);
        servicesListAdapter.notifyDataSetChanged();
    }

    public void createOrder(View view) {
        orderNumber.setText(orderNumber.getText().toString() + ((new Random()).nextInt(9000) + 1000));
        servicesListAdapter.clear();
        hoursNumber.setText("" + 1);

        Toast toast = Toast.makeText(this, "Заказ создан", Toast.LENGTH_LONG);
        toast.show();
    }
}
